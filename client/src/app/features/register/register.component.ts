import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { TextInputComponent } from "../../shared/components/text-input/text-input.component";

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatButton,
    RouterLink,
    CommonModule,
    RouterLink,
    TextInputComponent
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private snack = inject(SnackbarService);
  private route = inject(Router);
  hidePassword = true;
  validationErrors?: string[];

  registerForm = this.fb.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  get email() { return this.registerForm.get('email'); }
  get password() { return this.registerForm.get('password'); }
  get firstName() { return this.registerForm.get('firstName'); }
  get lastName() { return this.registerForm.get('lastName'); }

  onSubmit() {
    // console.log('clicked')
    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.snack.success('Registration successful - you can now login');
        this.route.navigateByUrl('/account/login')
      },
      error: errors => this.validationErrors = errors
    })
  }
}
