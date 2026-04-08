import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  nome: string = '';
  login: string = '';

  constructor(private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');

    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    // 🔥 Aqui você pode depois decodificar o token
    // por enquanto só mock pra exibir algo
    this.nome = 'Usuário Logado';
    this.login = 'admin';
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}