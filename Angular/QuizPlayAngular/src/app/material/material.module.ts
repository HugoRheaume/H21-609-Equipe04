import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
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
import { ClipboardModule } from '@angular/cdk/clipboard';
import { MatSortModule } from '@angular/material/sort';
import {DragDropModule} from '@angular/cdk/drag-drop';
import {MatTooltipModule} from '@angular/material/tooltip';

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
		MatTableModule,
		MatPaginatorModule,
        ClipboardModule,
		MatSortModule,
		DragDropModule,
    MatTooltipModule,

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
		MatTableModule,
		MatPaginatorModule,
		MatSortModule,
		ClipboardModule,
		DragDropModule,
    MatTooltipModule
	],
})
export class MaterialModule {}
