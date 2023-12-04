import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

type ProfileType = {
  givenName?: string,
  surname?: string,
  userPrincipalName?: string,
  id?: string
};

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: [],
  standalone: true
})
export class ProfileComponent implements OnInit {
  profile: ProfileType | undefined;

  constructor(
    private http: HttpClient
  ) { }

  ngOnInit() {
    if (typeof window !== "undefined") {
      this.getProfile(`${environment.apiBase}/api/blogs`);
    }
  }

  getProfile(url: string) {
    this.http.get(url)
      .subscribe(profile => {
        this.profile = profile;
      });
  }
}
