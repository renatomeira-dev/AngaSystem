
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginData = {
    login: '',
    senha: ''
  };

  constructor(private http: HttpClient, private router: Router) {}

  login() {
    this.http.post<any>('https://localhost:44322/api/usuario/login', this.loginData)
      .subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          console.log('token', res.token);
          console.log('teste: ', res);
          this.router.navigate(['/index']);
        },
        error: () => {
          alert('Login ou senha inválido');
        }
      });
  }
}

