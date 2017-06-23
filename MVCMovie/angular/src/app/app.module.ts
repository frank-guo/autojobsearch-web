import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';

import { AppComponent } from './app.component';
import { HomeComponent } from './home.component';
import { SelectModule } from 'ng2-select';
import { SearchCriteriaComponent } from './search-criteria.component';
import { SearchRuleComponent } from './search-rule.component';

@NgModule({
  declarations: [
      AppComponent,
      SearchCriteriaComponent,
      HomeComponent,
      SearchRuleComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    SelectModule,
    RouterModule.forRoot([
        {
            path: 'angularapp/search-rule/:id',
            component: SearchRuleComponent
        },
        {
            path: 'angularapp',
            component: HomeComponent
        }
    ])
  ],
  providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
  bootstrap: [AppComponent]
})
export class AppModule { }
