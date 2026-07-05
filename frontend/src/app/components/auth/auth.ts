import { Component, inject } from "@angular/core";
import { FormGroup, FormControl, ReactiveFormsModule } from "@angular/forms";
import { HttpClient } from "@angular/common/http";

interface LoginResponse {
	data: LoginData;
}

interface LoginData {
	id: number;
	username: string;
	token: string;
}

@Component({
  selector: "app-auth",
  imports: [ ReactiveFormsModule ],
  templateUrl: "./auth.html",
  styleUrl: "./auth.css",
})
export class Auth {
	private http = inject(HttpClient);

	loginForm = new FormGroup({
		login: new FormControl(''),
		password: new FormControl(''),
	});

	login() {
		// TODO(garipew): Abstract away API calls
		this.http.post<LoginResponse>("/api/auth/login", {
				       emailOrUsername: this.loginForm.value.login,
				       password: this.loginForm.value.password
			       }).subscribe({
				       next: (response) => {
					       console.log("Login ok! id: " + response.data.id + " username: " + response.data.username + " token: " + response.data.token);
				       },
				       error: (err) => {
					       console.error("Login failed", err);
				       }
			       });
	}
}
