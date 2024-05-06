import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ViaCepService } from '../../Services/ViaCep/via-cep.service';
import { CepModel } from '../../Models/CepModel';
import { LoadingService } from '../../Services/LoadingService';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shipping',
  templateUrl: './shipping.component.html',
  styleUrls: ['./shipping.component.css']
})

export class ShippingComponent implements OnInit {
  constructor(
    private ViaCepService: ViaCepService,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {
    this.cepDataSource = new MatTableDataSource<CepModel>;
    }
  private paginator!: MatPaginator;
  private sort!: MatSort;
  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes();
  }
  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes();
  }

  setDataSourceAttributes() {
    if (this.cepDataSource && this.paginator && this.sort)
      this.cepDataSource.paginator = this.paginator;
      this.cepDataSource.sort = this.sort;
  }

  searchForm!: FormGroup;
  resultForm!: FormGroup;
  cepDataSource!: MatTableDataSource<CepModel>;
  cep!: CepModel;
  displayedColumns: string[] = ['CEP', 'Logradouro', 'Complemento', 'Bairro', 'Localidade', 'UF', 'DDD', 'Número', 'actions'];
  ceps: CepModel [] = []; 


  ngOnInit(): void {
    this.initCepForm();
  }

  ngAfterViewInit() {
    this.cepDataSource.paginator = this.paginator;
    this.cepDataSource.sort = this.sort;

  }
  initCepForm(): void {
    this.searchForm = this.fb.group({
      cep: new FormControl(
        { value: null, disabled: false }, Validators.compose([Validators.minLength(8), Validators.required])),

      logradouro: '',
      complemento: '',
      bairro: '',
      localidade: '',
      uf: '',
      ddd: '',
      number: 0
    });
  }

  startFormResult(cep?: string, logradouro?: string, complemento?: string, bairro?: string, localidade?: string, uf?: string, ddd?: string, number?: number) {
    this.resultForm = new FormGroup({
      cep: new FormControl({ value: cep, disabled: false }, Validators.required),
      logradouro: new FormControl({ value: logradouro, disabled: false }, Validators.required),
      complemento: new FormControl({ value: complemento, disabled: false }),
      bairro: new FormControl({ value: bairro, disabled: false }, Validators.required),
      localidade: new FormControl({ value: localidade, disabled: false }),
      uf: new FormControl({ value: uf, disabled: false }, Validators.required),
      ddd: new FormControl({ value: ddd, disabled: false }),
      number: new FormControl({ value: number, disabled: false }),
    });
  }

  getCep() {
    const cep = this.searchForm.get('cep')?.value;
    const model = this.searchForm.getRawValue();
    this.loadingService.showLoading();
    this.ViaCepService.fetchCep(cep).subscribe({
      next: (res) => {
        this.startFormResult(res.cep, res.logradouro, res.complemento, res.bairro, res.localidade, res.uf, res.ddd);
        this.cep = res;
        this.loadingService.hideLoading();
      },
      error: (error: any) => {
        this.loadingService.hideLoading();
        this.toastr.error(error.message);
      }
    })
  }

  onCopyButtonClick() {
    const contentToCopy = document.getElementById('contentToCopy');

    if (!contentToCopy) {
      return;
    }
    const range = document.createRange();
    range.selectNode(contentToCopy);

    window.getSelection()?.addRange(range);

    document.execCommand('copy');

    window.getSelection()?.removeAllRanges();
  }

  onSave() {
    const formValue: CepModel = this.resultForm.getRawValue();
    this.ceps.push(formValue);
    this.cepDataSource.data = this.ceps;
    this.resultForm.reset();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.cepDataSource.filter = filterValue.trim().toLowerCase();

    if (this.cepDataSource.paginator) {
      this.cepDataSource.paginator.firstPage();
    }
  }
}
