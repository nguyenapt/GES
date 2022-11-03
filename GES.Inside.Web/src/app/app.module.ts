import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { GssCompanyResearchList } from "./gssCompanyResearch/inside.gssCompanyResearchList";
import { RouterModule, Routes } from "@angular/router";
import { ClaimRoutingComponent } from "./claim-routing";
import {APP_BASE_HREF, PlatformLocation} from "@angular/common";
import { GssCompanyResearchDetailComponent } from './gss-company-research-detail/gss-company-research-detail.component';
import { GssSourceComponent } from './gss-source/gss-source.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { GssInternalCommentComponent } from './gss-internal-comment/gss-internal-comment.component';
import { GssAssessmentComponent } from './gss-assessment/gss-assessment.component';
import { GssGeneralComponent } from './gss-general/gss-general.component';
import { GssUpgradeCriteriaComponent } from './gss-upgrade-criteria/gss-upgrade-criteria.component';
import { GssCompanyContactComponent } from './gss-company-contact/gss-company-contact.component';
import { GssInternalCommentDialogComponent } from './gss-internal-comment-dialog/gss-internal-comment-dialog.component';
import { GssPrincipleComponent } from './gss-principle/gss-principle.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule} from "ngx-toastr";
import { A11yModule } from '@angular/cdk/a11y';
import { BidiModule } from '@angular/cdk/bidi';
import { ObserversModule } from '@angular/cdk/observers';
import { OverlayModule } from '@angular/cdk/overlay';
import { PlatformModule } from '@angular/cdk/platform';
import { PortalModule } from '@angular/cdk/portal';
import { ScrollDispatchModule } from '@angular/cdk/scrolling';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import {
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
} from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { GssSourceDialogComponent } from './gss-source-dialog/gss-source-dialog.component';
import { AttrClickToEditDirective } from './directive/attr-click-to-edit.directive';
import { GssAssessmentUngpBusinessHumanRightDialogComponent } from './gss-assessment-ungp-business-human-right-dialog/gss-assessment-ungp-business-human-right-dialog.component';
import { GssAssessmentOtherRelatedConventionDialogComponent } from './gss-assessment-other-related-convention-dialog/gss-assessment-other-related-convention-dialog.component';
import { GssAssessmentOcedDialogComponent } from './gss-assessment-oced-dialog/gss-assessment-oced-dialog.component';

@NgModule({
    exports: [
        // CDK
        A11yModule,
        BidiModule,
        ObserversModule,
        OverlayModule,
        PlatformModule,
        PortalModule,
        ScrollDispatchModule,
        CdkStepperModule,
        CdkTableModule,
        CdkTreeModule,

        // Material
        MatAutocompleteModule,
        MatBadgeModule,
        MatBottomSheetModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatCardModule,
        MatCheckboxModule,
        MatChipsModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatGridListModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatProgressBarModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatRippleModule,
        MatSelectModule,
        MatSidenavModule,
        MatSliderModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatSortModule,
        MatStepperModule,
        MatTableModule,
        MatTabsModule,
        MatToolbarModule,
        MatTooltipModule,
        MatTreeModule,
    ]
})
export class MaterialModule { }

const appRoutes: Routes = [
    { path: 'GSSResearch/CompanyList', component: GssCompanyResearchList },
    { path: 'GSSResearch/CompanyDetails/:claimid', component: GssCompanyResearchDetailComponent }
];

@NgModule({
    declarations: [
        AppComponent,
        GssCompanyResearchList,
        ClaimRoutingComponent,
        GssCompanyResearchDetailComponent,
        GssSourceComponent,
        GssInternalCommentComponent,
        GssAssessmentComponent,
        GssGeneralComponent,
        GssUpgradeCriteriaComponent,
        GssCompanyContactComponent,
        GssInternalCommentDialogComponent,
        GssAssessmentUngpBusinessHumanRightDialogComponent,
        GssAssessmentOtherRelatedConventionDialogComponent,
        GssAssessmentOcedDialogComponent,
        GssSourceDialogComponent,
        GssPrincipleComponent,
        AttrClickToEditDirective
    ],
    imports: [
      BrowserModule, FormsModule, HttpClientModule, BrowserAnimationsModule,ToastrModule.forRoot(), RouterModule.forRoot(appRoutes),
        FormsModule,
        MaterialModule,
        MatDatepickerModule, MatInputModule, MatNativeDateModule,
        ReactiveFormsModule, BrowserAnimationsModule, FlexLayoutModule,
        HttpClientModule, 
        ToastrModule.forRoot(),
        RouterModule.forRoot(appRoutes)
    ],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
    bootstrap: [ClaimRoutingComponent],
    entryComponents: [
        GssInternalCommentDialogComponent,
        GssSourceDialogComponent,
        GssAssessmentUngpBusinessHumanRightDialogComponent,
        GssAssessmentOtherRelatedConventionDialogComponent,
        GssAssessmentOcedDialogComponent
    ]
})
export class AppModule { }
