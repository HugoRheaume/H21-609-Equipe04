import { AuthService } from './../services/auth.service';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    let loginStatus = await this.authService.isAuthenticated();
    if (loginStatus) return true;

    this.router.navigate(['/'], {
      queryParams: { returnUrl: state.url }
    });
    localStorage.clear();
  }
}
