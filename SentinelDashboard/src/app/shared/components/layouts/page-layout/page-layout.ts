import { Component, input } from '@angular/core';
import { Button } from 'primeng/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-page-layout',
  imports: [Button, RouterLink],
  templateUrl: './page-layout.html',
  styleUrl: './page-layout.css',
  host: {
    class: 'flex flex-col items-start gap-1',
  },
})
export class PageLayout {
  showBackButton = input(true);
}
