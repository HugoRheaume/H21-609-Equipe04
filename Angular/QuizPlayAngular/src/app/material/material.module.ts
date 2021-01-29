import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDividerModule } from '@angular/material/divider';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		BrowserAnimationsModule,
		MatFormFieldModule,
		MatCardModule,
		MatButtonModule,
		MatToolbarModule,
		MatIconModule,
		MatGridListModule,
		MatInputModule,
		MatDividerModule,
		MatSelectModule,
		MatDialogModule,
		MatProgressSpinnerModule,
		MatCheckboxModule,
		MatRadioModule,
		MatSlideToggleModule,
	],
	exports: [
		BrowserAnimationsModule,
		MatFormFieldModule,
		MatCardModule,
		MatButtonModule,
		MatToolbarModule,
		MatIconModule,
		MatGridListModule,
		MatInputModule,
		MatDividerModule,
		MatSelectModule,
		MatDialogModule,
		MatProgressSpinnerModule,
		MatCheckboxModule,
		MatRadioModule,
		MatSlideToggleModule,
	],
})
export class MaterialModule {}