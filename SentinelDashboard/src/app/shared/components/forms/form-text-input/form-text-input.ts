import { Component, input } from '@angular/core';
import { FloatLabelModule } from 'primeng/floatlabel';
import { Message, MessageModule } from 'primeng/message';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { PasswordModule } from 'primeng/password';
import { InputIconModule } from 'primeng/inputicon';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

export type TextInputType = 'text' | 'email' | 'password';

@Component({
  selector: 'app-form-text-input',
  imports: [
    InputTextModule,
    MessageModule,
    IconFieldModule,
    FloatLabelModule,
    InputIconModule,
    ReactiveFormsModule,
    PasswordModule,
  ],
  templateUrl: './form-text-input.html',
  styleUrl: './form-text-input.css',
})
export class FormTextInput<T = string> {
  control = input.required<FormControl<T>>();
  label = input<string>();
  type = input<TextInputType>('text');
  errors = input<Record<string, string>>({});
  icon = input<string>();

  isInvalid() {
    const ctrl = this.control();
    return ctrl.invalid && (ctrl.dirty || ctrl.touched);
  }

  getId() {
    return `${this.label()?.toLocaleLowerCase()}-input`;
  }

  getErrorMessage() {
    const ctrl = this.control();
    if (!ctrl.errors) return '';
    const firstErrorKey = Object.keys(ctrl.errors)[0];
    return this.errors()[firstErrorKey] || 'Invalid input';
  }
}
