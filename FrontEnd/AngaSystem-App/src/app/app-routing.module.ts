import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { UsuarioComponent } from './pages/usuario/usuario.component';
import { IndexComponent } from './pages/index/index.component';

const routes: Routes = [
  { path: '', component: LoginComponent }, 
  { path: 'login', component: LoginComponent } ,
  { path: 'usuario', component: UsuarioComponent },
  {
    path: 'index', component: IndexComponent,
    children: [
      { path: 'usuario', component: UsuarioComponent },
      { path: 'fornecedores', component: UsuarioComponent },
      { path: 'clientes', component: UsuarioComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
