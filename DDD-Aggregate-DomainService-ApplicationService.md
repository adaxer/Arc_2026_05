# DDD – Aggregate, Domain Service und Application Service

## Was gehört in ein Aggregat?

Ein Aggregat enthält vor allem:

- Fachlichen Zustand
- Fachliche Regeln (Business Rules)
- Invarianten
- Value Objects
- Kind-Entities innerhalb der Aggregate-Grenze

Beispiel:

```csharp
public class Loan
{
    public LoanId Id { get; }
    public DateOnly DueDate { get; private set; }
    public LoanStatus Status { get; private set; }

    public void ExtendUntil(DateOnly newDueDate)
    {
        if (Status == LoanStatus.Returned)
            throw new DomainException("Returned loans cannot be extended.");

        DueDate = newDueDate;
    }
}
```

Nicht ins Aggregat gehören:

- Repository-Instanzen
- DbContext
- DTO-Mapping
- HTTP-Aufrufe
- Logging
- E-Mail-Versand
- Transaktionssteuerung

---

## Wie arbeitet ein Application Service?

Der Application Service orchestriert einen Use Case:

```csharp
public async Task ExtendLoan(ExtendLoanCommand cmd)
{
    var loan = await loanRepository.GetByIdAsync(cmd.LoanId);

    loan.ExtendUntil(cmd.NewDate);

    await unitOfWork.SaveChangesAsync();
}
```

Aufgaben:

- Aggregate laden
- Domainlogik aufrufen
- Speichern
- Infrastruktur verwenden
- Workflow koordinieren

---

## Wird das Aggregat injiziert?

Nein.

Das Aggregat wird normalerweise:

- aus einem Repository geladen oder
- neu instanziiert

Beispiel:

```csharp
var loan = await loanRepository.GetByIdAsync(id);
```

oder

```csharp
var loan = Loan.Create(...);
```

Ein Aggregat ist kein Service.

---

## Hat ein Aggregat Abhängigkeiten nach außen?

Idealerweise nein.

Nicht:

```csharp
public Loan(ILoanRulesService rules)
{
}
```

Sondern:

```csharp
loan.ExtendUntil(newDate, rules);
```

oder

```csharp
policy.EnsureCanExtend(loan, newDate);
loan.ExtendUntil(newDate);
```

DDD bevorzugt zustandsbasierte Objekte ohne Infrastruktur-Abhängigkeiten.

---

## Dynamische Regeln

### Variante 1: Regelobjekt übergeben

```csharp
loan.ExtendUntil(newDate, rules);
```

### Variante 2: Domain Service

Wenn die Regel mehrere Aggregate betrifft:

```csharp
loanPolicy.EnsureCanBorrow(member, activeLoanCount);
```

### Variante 3: Specification / Policy

```csharp
loan.ExtendUntil(newDate, extensionPolicy);
```

---

## Kann ein Aggregat gespeichert werden?

Ja.

Ein Aggregat ist normalerweise persistierbar und wird häufig über EF Core gespeichert.

```csharp
var loan = await loanRepository.GetByIdAsync(id);

loan.ExtendUntil(newDate);

await unitOfWork.SaveChangesAsync();
```

Ein Aggregat ist:

- zustandsbehaftet
- persistierbar
- fachlich intelligent

---

## Ist ein Aggregat eine Entity?

Der Aggregate Root ist normalerweise auch eine Entity.

Beispiel:

```csharp
public class Loan
{
    public LoanId Id { get; }
}
```

DDD unterscheidet:

```text
Entity
    = Objekt mit Identität

Aggregate
    = Konsistenzgrenze

Aggregate Root
    = äußere Entity des Aggregats
```

Nicht jede Entity ist ein Aggregate Root.

Jeder Aggregate Root ist normalerweise eine Entity.

---

## Muss ein Aggregat eine eigene Tabelle haben?

Nein.

DDD koppelt das Domainmodell nicht an die Datenbankstruktur.

Ein Aggregat kann:

- einer Tabelle entsprechen
- mehreren Tabellen entsprechen
- als Dokument gespeichert werden
- per Event Sourcing gespeichert werden

Beispiel:

```text
Loan
 ├─ LoanExtensions
 └─ LoanNotes
```

Mögliches Mapping:

```text
Loans
LoanExtensions
LoanNotes
```

Fachlich bleibt es trotzdem ein Aggregat.

---

## Unterschied zwischen Domain Service und Application Service

### Application Service

Frage:

```text
Was passiert wann?
```

Aufgaben:

- Workflow koordinieren
- Repositories verwenden
- Aggregate laden
- Speichern
- Transaktionen steuern

Beispiel:

```csharp
public async Task ExtendLoan(...)
{
    var loan = await repository.GetByIdAsync(...);

    loan.ExtendUntil(...);

    await unitOfWork.SaveChangesAsync();
}
```

### Domain Service

Frage:

```text
Was ist fachlich erlaubt?
```

Enthält Fachlogik, die nicht natürlich in ein einzelnes Aggregat passt.

Beispiel:

```csharp
public class LoanPolicy
{
    public void EnsureCanBorrow(
        Member member,
        int activeLoanCount)
    {
        if (activeLoanCount >= 5)
            throw new DomainException(...);
    }
}
```

---

## Typischer Ablauf

```text
Controller / API
    -> Application Service
        -> lädt Aggregate
        -> ruft Domain Services auf
        -> ruft Aggregate auf
        -> speichert
```

---

## Merksätze

### Aggregat

```text
Fachlicher Zustand + Regeln + Konsistenzgrenze
```

### Application Service

```text
Orchestriert den Use Case
```

### Domain Service

```text
Enthält fachliche Regeln,
die nicht in ein einzelnes Aggregat passen
```
