import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Toaster } from "./common/toaster/toaster";
import { LoadingBarComponent } from './common/loading-bar/loading-bar';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, Toaster,LoadingBarComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.css'], 
})
export class App {

}
