import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Signup } from './auth/signup/signup';
import { Dashboard } from './dashboard/dashboard';
import { CreateTest } from './test/create-test/create-test';
import { ManageTest } from './test/manage-test/manage-test';
import { GetTest } from './test/get-test/get-test';

export const routes: Routes = [
    { path: '', component: Dashboard },
    { path: 'login', component: Login },
    { path: 'signup', component: Signup },
    { path: 'create-test', component: CreateTest },
    { path: 'manage-test', component: ManageTest },
    { path: 'get-test', component: GetTest },
    { path: '**', component:Dashboard}
];
