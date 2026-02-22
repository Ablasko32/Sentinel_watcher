import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { TabsModule } from 'primeng/tabs';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { PageLayout } from '../../../../../shared/components/layouts/page-layout/page-layout';
import { filter } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-settings-layout',
  imports: [TabsModule, RouterOutlet, PageLayout, RouterLink],
  templateUrl: './settings-layout.html',
  styleUrl: './settings-layout.css',
})
export class SettingsLayout implements OnInit {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);
  readonly tabs = [
    { label: 'Api Keys', route: 'api-keys', icon: 'pi pi-key' },
    { label: 'Users', route: 'users', icon: 'pi pi-user' },
  ];

  activeRoute = signal<string>(this.tabs[0].route ?? '');

  ngOnInit(): void {
    this.updateActiveRoute();
    this.router.navigate([this.activeRoute()], { relativeTo: this.route });
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => this.updateActiveRoute());
  }

  private updateActiveRoute() {
    const url = this.router.url;
    const activeTab = this.tabs.find((tab) => url.includes(tab.route));

    this.activeRoute.set(activeTab?.route ?? this.tabs[0].route);
  }
}
