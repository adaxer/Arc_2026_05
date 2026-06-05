import { Component, ChangeDetectorRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../auth.service';
import { firstValueFrom } from 'rxjs';

@Component({
  standalone: false,
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  email = '';
  password = '';
  invalid = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  async login() {
    this.invalid = false;
    try {
      await firstValueFrom(this.authService.login(this.email, this.password));

      // Get return URL, but avoid redirecting back to login
      let returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

      // If returnUrl is login or register, go to home instead
      if (returnUrl === '/login' || returnUrl === '/register' || returnUrl.startsWith('/login') || returnUrl.startsWith('/register')) {
        returnUrl = '/';
      }

      await this.router.navigateByUrl(returnUrl);
    } catch (error) {
      console.error('Login error:', error);
      this.invalid = true;
      this.cdr.detectChanges();
    }
  }
}
