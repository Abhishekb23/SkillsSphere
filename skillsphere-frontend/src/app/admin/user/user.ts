import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconButton } from "@angular/material/button";
import { UserService } from '../../services/user.service';
import { ToasterService } from '../../services/toaster.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatIconModule,
    MatMenuModule, MatIconButton],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User implements AfterViewInit {
  displayedColumns: string[] = ['profile', 'username', 'email', 'role', 'status', 'action'];
  dataSource = new MatTableDataSource<any>([]);
  searchTerm = '';

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private authService: AuthService, 
    private userService: UserService, 
    private toasterService: ToasterService,
    private router: Router
  ) {
    if(!this.authService.isAdmin()){
      this.router.navigate(['/']);
    }
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers().subscribe({
      next: (result) => {
        this.dataSource.data = result;
      },
      error: () => {
        this.toasterService.error("Unable to get users");
      }
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getRoleName(role: number): string {
    switch (role) {
      case 1:
        return 'Learner';
      case 2:
        return 'Instructor';
      case 3:
        return 'Admin';
      default:
        return 'User';
    }
  }
}
