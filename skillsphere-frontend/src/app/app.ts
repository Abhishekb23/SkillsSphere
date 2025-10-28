import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toaster } from "./common/toaster/toaster";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, Toaster],
  templateUrl: './app.html',
  styleUrls: ['./app.css'], 
})
export class App {

}
