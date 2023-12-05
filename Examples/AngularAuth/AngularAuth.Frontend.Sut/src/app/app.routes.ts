import { Route, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { FailedComponent } from './failed/failed.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';

export const routes: Route[] = [
    {
        path: 'profile',
        component: ProfileComponent,
        canActivate: [MsalGuard]
    },
    {
        path: '',
        component: HomeComponent
    },
    {
        path: 'login-failed',
        component: FailedComponent
    }
    ];
