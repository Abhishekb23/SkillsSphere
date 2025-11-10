import { Routes } from '@angular/router';
import { Login } from './auth/login/login';
import { Signup } from './auth/signup/signup';
import { Dashboard } from './dashboard/dashboard';
import { CreateTest } from './test/create-test/create-test';
import { GetTest } from './test/get-test/get-test';
import { Profile } from './common/profile/profile';
import { Result } from './test/result/result';
import { User } from './admin/user/user';
import { Test } from './learner/test/test';
import { ManageTest } from './test/manage-test/manage-test';

export const routes: Routes = [
    { path: '', component: Dashboard },
    { path: 'login', component: Login },
    { path: 'signup', component: Signup },
    { path: 'create-test', component: CreateTest },
    { path: 'tests', component: Test },
    { path: 'manage-test', component: ManageTest },
    { path: 'get-test/:testId', component: GetTest },
    { path: 'profile', component: Profile },
    { path: 'users', component: User },
    { path: 'result', component: Result },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
