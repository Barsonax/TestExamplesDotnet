import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

type BlogType = {
  blogId: number,
  url: string,
  posts: PostType[],
};

type PostType = {
  postId: number,
  title: string,
  content: string,
};

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: [],
  standalone: true
})
export class ProfileComponent implements OnInit {
  profile: BlogType | undefined;

  constructor(
    private http: HttpClient
  ) { }

  ngOnInit() {
    if (typeof window !== "undefined") {
      this.getProfile(`${environment.apiBase}/api/blogs`);
    }
  }

  getProfile(url: string) {
    this.http.get<BlogType[]>(url)
      .subscribe(profile => {
        this.profile = profile[0];
      });
  }
}
