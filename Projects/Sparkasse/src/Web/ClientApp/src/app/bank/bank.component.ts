import { Component, OnInit, signal } from '@angular/core';
import { BankAccountsClient, OpenAccountCommand } from '../web-api-client';

interface BankAccountTransaction {
  timestamp: Date;
  amount?: number;
  balance?: number;
  description: string;
}

@Component({
  standalone: false,
  selector: 'app-bank',
  templateUrl: './bank.component.html',
  styleUrls: ['./bank.component.scss']
})
export class BankComponent implements OnInit {
  accountId = signal<string | null>(null);
  balance = signal<number>(0);
  transactions = signal<BankAccountTransaction[]>([]);
  initialBalance = signal<number>(100);
  amount = signal<number>(0);
  statusMessage = signal<string>('');
  errorMessage = signal<string>('');
  isLoading = signal<boolean>(false);
  hasAccount = signal<boolean>(false);

  constructor(private bankClient: BankAccountsClient) { }

  ngOnInit(): void {
    this.checkAccount();
  }

  checkAccount(): void {
    const storedAccountId = localStorage.getItem('bankAccountId');
    if (storedAccountId) {
      this.isLoading.set(true);
      this.bankClient.balance(storedAccountId).subscribe({
        next: balance => {
          this.accountId.set(storedAccountId);
          this.balance.set(balance);
          this.hasAccount.set(true);
          this.statusMessage.set('Konto geladen.');
          this.isLoading.set(false);
        },
        error: error => {
          console.error('Account check failed:', error);
          localStorage.removeItem('bankAccountId');
          this.hasAccount.set(false);
          this.errorMessage.set('Kontoprüfung fehlgeschlagen. Bitte neues Konto erstellen.');
          this.isLoading.set(false);
        }
      });
    }
  }

  createAccount(): void {
    if (this.initialBalance() < 0) {
      this.errorMessage.set('Anfangssaldo darf nicht negativ sein.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.statusMessage.set('');

    const command = new OpenAccountCommand({ initialBalance: this.initialBalance() });
    this.bankClient.openBankAccount(command).subscribe({
      next: accountId => {
        this.accountId.set(accountId);
        this.balance.set(this.initialBalance());
        this.hasAccount.set(true);
        this.statusMessage.set(`Konto erfolgreich erstellt. ID: ${accountId}`);

        // Store account ID in localStorage for session persistence
        localStorage.setItem('bankAccountId', accountId);

        this.loadStatement();
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(`Fehler beim Erstellen des Kontos: ${error.message || error}`);
        this.isLoading.set(false);
      }
    });
  }

  deposit(): void {
    const currentAmount = this.amount();

    if (currentAmount <= 0) {
      this.errorMessage.set('Bitte einen gültigen Betrag eingeben (> 0).');
      return;
    }

    if (!this.accountId()) {
      this.errorMessage.set('Kein Konto vorhanden.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.statusMessage.set('');

    this.bankClient.deposit(this.accountId()!, currentAmount).subscribe({
      next: newBalance => {
        this.balance.set(newBalance);
        this.statusMessage.set(`Einzahlung von ${currentAmount.toFixed(2)} € erfolgreich. Neuer Kontostand: ${newBalance.toFixed(2)} €`);
        this.amount.set(0);
        this.loadStatement();
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(`Fehler bei der Einzahlung: ${error.message || error}`);
        this.isLoading.set(false);
      }
    });
  }

  withdraw(): void {
    const currentAmount = this.amount();

    if (currentAmount <= 0) {
      this.errorMessage.set('Bitte einen gültigen Betrag eingeben (> 0).');
      return;
    }

    if (!this.accountId()) {
      this.errorMessage.set('Kein Konto vorhanden.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.statusMessage.set('');

    this.bankClient.withdraw(this.accountId()!, currentAmount).subscribe({
      next: newBalance => {
        this.balance.set(newBalance);
        this.statusMessage.set(`Auszahlung von ${currentAmount.toFixed(2)} € erfolgreich. Neuer Kontostand: ${newBalance.toFixed(2)} €`);
        this.amount.set(0);
        this.loadStatement();
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(`Fehler bei der Auszahlung: ${error.message || error}`);
        this.isLoading.set(false);
      }
    });
  }

  closeAccount(): void {
    if (!this.accountId()) {
      this.errorMessage.set('Kein Konto vorhanden.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.statusMessage.set('');

    this.bankClient.close(this.accountId()!).subscribe({
      next: finalBalance => {
        this.statusMessage.set(`Konto wurde geschlossen. Finaler Kontostand: ${finalBalance.toFixed(2)} €`);
        this.hasAccount.set(false);
        this.accountId.set(null);
        this.balance.set(0);
        this.transactions.set([]);

        // Remove from localStorage
        localStorage.removeItem('bankAccountId');

        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(`Fehler beim Schließen des Kontos: ${error.message || error}`);
        this.isLoading.set(false);
      }
    });
  }

  getStatement(): void {
    this.loadStatement();
  }

  private loadStatement(): void {
    if (!this.accountId()) {
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.bankClient.statement(this.accountId()!).subscribe({
      next: statement => {
        const transactions: BankAccountTransaction[] = statement.map(s => ({
          timestamp: new Date(s.timestamp!),
          amount: s.amount ?? undefined,
          balance: s.balance ?? undefined,
          description: s.amount !== undefined && s.amount !== null 
            ? (s.amount >= 0 ? 'Einzahlung' : 'Auszahlung')
            : 'Unbekannt'
        })).sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());

        this.transactions.set(transactions);
        this.statusMessage.set(`Kontoauszug aktualisiert am ${new Date().toLocaleString('de-DE')}`);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(`Fehler beim Laden des Kontoauszugs: ${error.message || error}`);
        this.isLoading.set(false);
      }
    });
  }
}
