import { Component, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { EsignSignerTemplateLinkOutputWebDto, MstEsignActiveDirectoryServiceProxy, MstEsignColorServiceProxy, MstEsignSignerTemplateServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectionSettingsModel } from '@syncfusion/ej2-angular-grids';
import { ListViewComponent } from '@syncfusion/ej2-angular-lists';
import { TabComponent } from '@syncfusion/ej2-angular-navigations';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Listbox } from 'primeng/listbox';
import { TabView } from 'primeng/tabview';
import { finalize } from 'rxjs';

@Component({
    selector: 'search-signer',
    templateUrl: './search-signer.component.html',
    styleUrls: ['./search-signer.component.less']
})
export class SearchSignerComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal!: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('listViewSigner') listViewSigner: TabView;
    @ViewChild('listViewCC') listViewCC: TabView;
    @ViewChild('listView') listview: Listbox;
    @ViewChild('user') tab: TabComponent;
    @Input() listSigner;
    searchFilter;
    dialogWidth: string = '35%';
    dialogHeight: string = '81%';
    visible: Boolean = false;
    visibleInfo: Boolean = false;
    animationSettings: Object = { effect: 'FadeZoom' };
    selectedSigner: any[] = [];
    selectedCC: any[] = [];
    searchResult: any[] = [];
    selectedAll: any[] = [];
    selectedAllTmp: any[] = [];
    searchResultTemplate: {
        id: number,
        localName: string,
        text: string,
        addCC: string,
        internationalName: string,
        child: any []
    }[] = [];
    activeType: number = -1;
    cc;
    isUseTemplate = false;
    allColor;
    loading = false;
    isAddCC = false;
    selectSignerInfo;
    isSelectAll = false;
    draggedSigner: any;
    draggedCC: any;
    constructor(
        injector: Injector,
        private _esignTemplate: MstEsignSignerTemplateServiceProxy,
        private _activeDirectoryService: MstEsignActiveDirectoryServiceProxy,
        private _colorService : MstEsignColorServiceProxy,
        private _signerInfo: MstEsignActiveDirectoryServiceProxy
    ) {
        super(injector);

    }

    ngOnInit() {
        this._colorService.getAllColor("","").subscribe(res => {
            this.allColor = res.items;
        });
        this._activeDirectoryService.getAllSignerByGroup(
            this.searchFilter,
            -1,
            0,
            10000000
        )
        .subscribe(res => {
            this.listSigner = res?.items?.map(e => {
                return {
                    ...e,
                    imgUrl: e.imageUrl,
                }
            });
        });
    }

    submit() {
        this.modalSave.emit({signers: this.selectedSigner, addCC: this.selectedCC, isUseTemplate: this.isUseTemplate});
        this.isAddCC = false;
        this.selectedCC = [];
        this.selectedSigner = [];
        this.hide();
    }

    show(inputSigner = [], selectedCC = []) {
        this.tab.select(0);
        this.modal.show();
        this.selectedAll = inputSigner?.concat(selectedCC);
        this.selectedAllTmp = inputSigner?.concat(selectedCC);
        this.activeType = -1;
        this.searchResult = this.listSigner?.slice(0, 60).map(x => {
            return {
                ...x,
                imgUrl: x.imgUrl,
            }
        });
        this.selectedCC = selectedCC;
        this.selectedSigner = inputSigner;
    }

    hide() {
        this.modal.hide();
        this.selectedSigner = [];
        this.searchResult = [];
        this.searchResultTemplate = [];
        this.searchFilter = '';
        this.cc = '';
        this.activeType = -1;
        this.isUseTemplate = false;
    }

    changeKey(event) {
        this.searchFilter = event.target.value;
        if (event.keyCode == 13) {
            this.searchAll(this.activeType);
        }
    }

    searchAll(type) {
        // if(!this.searchFilter && type == 0){
        //     this.searchResult = this.listSigner?.slice(0, 60)?.map(x => {
        //         return {
        //             ...x,
        //             imageUrl: x.imgUrl,
        //         }
        //     });
        //     return;
        // }
        if (type != 0) {
            this._activeDirectoryService.getAllSignerByGroup(
                this.searchFilter,
                this.activeType,
                0,
                50
            )
            .pipe(finalize(() => this.loading = false))
            .subscribe(res => {
                this.searchResult = res?.items?.map(e => {
                    return {
                        ...e,
                        imgUrl: e.imageUrl,
                    }});
            });
        }
        else {
            this._esignTemplate.getListTemplateForUserWeb(this.searchFilter)
            .pipe(finalize(() => {
                this.loading = false;
            }))
            .subscribe(res => {
                this.searchResultTemplate = res.items.map(x => {
                    return {
                        id: x.id,
                        localName: x.localName,
                        text: x.localName,
                        addCC: x.addCC,
                        internationalName: x.internationalName,
                        child: x.listSigner.map(e => {
                            return {
                                ...e,
                                imgUrl: e.imageUrl,
                            }})
                    }
                });
            });
        }
    }

    select(event) {
        this.loading = true;
        if(this.isUseTemplate){
            this.isUseTemplate = false;
            this.cc = '';
            this.selectedSigner = [];
        }
        if (event?.isChecked && !this.selectedSigner?.some(x => x?.id == event?.data.id)) {
            if(!this.isAddCC){
                if(event?.data?.id == this.appSession.userId){
                    this.selectedSigner.forEach(x => x.signingOrder = x.signingOrder + 1);
                    this.selectedSigner.unshift({...event?.data, signingOrder: 1});
                }
                else {
                    let maxSigningOrder = this.selectedSigner?.length > 0 ? Math.max(...this.selectedSigner.map(x => x.signingOrder)) : 0;
                    this.selectedSigner.push({...event?.data, signingOrder: maxSigningOrder + 1});
                }
            }
            else {
                this.selectedCC.push(event?.data);
            }
        }
        else {
            this.selectedSigner = this.selectedSigner.filter(x => x.id != event?.data.id);
            this.selectedCC = this.selectedCC.filter(x => x.id != event?.data.id);
        }
        this.loading = false;
    }

    selectTemplate(event) {
        if(event?.data?.localName){
            this.loading = true;
            this.selectedSigner = event?.data?.child;
            this.isUseTemplate = true;
            this.loading = false;
            this.selectedCC = this.listSigner?.filter(x => event?.data?.addCC?.includes(x.email))?.map(x => {
                return {
                    ...x,
                    isCC: true,
                    imageUrl: x.imgUrl,
                }
            });
            this.selectedAll = this.selectedSigner?.concat(this.selectedCC);
            this.selectedAllTmp = this.selectedAll;
        }
    }

    onCloseSigner(event) {
        //remove index
        if(event?.index === 0 || event?.index){
            let removeItem = this.selectedSigner[event?.index];
            this.selectedSigner = this.selectedSigner.filter(x => x.id !== removeItem?.id);
            this.selectedAll = this.selectedAll.filter(x => x.id !== removeItem?.id);
            this.selectedAllTmp = this.selectedAllTmp.filter(x => x.id != removeItem?.id);
            this.listview.cd.detectChanges();
        }
    }

    onChangeType(event) {
        this.isSelectAll = false;
        this.activeType = this.getActiveType(event?.selectedIndex);
        this.searchAll(this.activeType);
    }

    getActiveType(activeIndex){
        switch (activeIndex) {
            case 0:
                return -1;
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 4;
            case 4:
                return 0;
            case 5:
                return 3;
            default:
                return -1;
        }
    }

    changeAddType(event){
        this.isAddCC = event;
    }

    onCloseCC(event) {
        //remove index
        if(event?.index === 0 || event?.index){
            let removeItem = this.selectedCC[event?.index];
            this.selectedCC = this.selectedCC.filter(x => x.id != removeItem?.id);
            this.selectedAll = this.selectedAll.filter(x => x.id != removeItem?.id);
            this.selectedAllTmp = this.selectedAllTmp.filter(x => x.id != removeItem?.id);
            this.listview.cd.detectChanges();
        }
    }

    showInfo(id){
        this._signerInfo.getUserInformationById(id).subscribe(res => {
            this.selectSignerInfo = res;
        });
        this.visibleInfo = true;
    }

    onChangeSelect(event){
        if(this.selectedAllTmp?.some(x => x.id == event?.option.id)){
            this.selectedCC = this.selectedCC.filter(x => x.id != event.option.id);
            this.selectedSigner = this.selectedSigner.filter(x => x.id != event.option.id);
        }
        else{
            if(this.isAddCC){
                if(event?.option?.id == this.appSession.userId){
                    this.notify.warn(this.l('YouMustBeSigner'));
                    this.selectedAll = this.selectedSigner?.concat(this.selectedCC).filter(x => x.id != this.appSession.userId);
                    this.listview.cd.detectChanges();
                }
                else {
                    this.selectedCC.push(event.option);
                }
            }
            else {
                if(event?.option?.id == this.appSession.userId){
                    this.selectedSigner.forEach(x => x.signingOrder = x.signingOrder + 1);
                    this.selectedSigner.unshift({...event.option, signingOrder: 1});
                }
                else {
                    this.selectedSigner.push({
                        ...event.option,
                        signingOrder: this.selectedSigner?.length > 0 ? Math.max(...this.selectedSigner.map(x => x.signingOrder)) + 1 : 1
                    });
                }
            }
        }
        this.selectedAllTmp = this.selectedSigner?.concat(this.selectedCC);
    }

    onSelectAllChange(event){
        if(event?.checked){
            if(this.isAddCC){
                this.selectedCC = this.searchResult.filter(e => !this.selectedSigner?.some(x => x.id == e.id) && !this.selectedCC?.some(x => x.id == e.id));
            }
            else {
                this.searchResult.filter(e => !this.selectedCC?.some(x => x.id == e.id) &&  !this.selectedSigner?.some(y => y.id == e.id)).map(x => {
                    this.selectedSigner?.push({
                        ...x,
                        signingOrder: this.selectedSigner?.length > 0 ? Math.max(...this.selectedSigner.map(x => x.signingOrder)) + 1 : 1
                    });
                });
            }
            if(!this.selectedCC?.some(x => x.id == this.appSession.userId) && this.selectedSigner.some(x => x.id == this.appSession.userId)){
                this.selectedSigner = this.selectedSigner.filter(x => x.id != this.appSession.userId).map(x => {
                    return {
                        ...x,
                        signingOrder: x.signingOrder + 1
                    }
                });
                if(this.listSigner?.some(x => x.id == this.appSession.userId)){
                    this.selectedSigner.unshift({
                        ...this.listSigner.find(x => x.id == this.appSession.userId),
                        signingOrder: 1});
                }
            }
            this.selectedAllTmp = this.searchResult;
            this.selectedAll = this.searchResult;
        }
        else
        {
            this.selectedCC = [];
            this.selectedSigner = [];
            this.selectedAllTmp = [];
            this.selectedAll = [];
        }
    }
    onScroll(){
        let pos = this.round((document.getElementById('listView').scrollTop || document.body.scrollTop) + document.getElementById('listView').offsetHeight);
        let match = this.round(document.getElementById('listView').scrollHeight * 0.68);
        let max = this.round(document.getElementById('listView').scrollHeight);
        if (pos == match && this.searchResult.length < this.listSigner.length && this.activeType == -1 && !this.searchFilter) {
            this.searchResult = this.searchResult.concat(this.listSigner.filter(e => !this.searchResult?.some(x => x.id == e.id)).slice(0, 111).map(x => {
                return {
                    ...x,
                    imageUrl: x.imgUrl,
                }
            }));
        }
        else if(pos == max && this.searchResult.length < this.listSigner.length && this.activeType == -1  && !this.searchFilter){
            this.searchResult = this.searchResult.concat(this.listSigner.filter(e => !this.searchResult?.some(x => x.id == e.id)).map(x => {
                return {
                    ...x,
                    imageUrl: x.imgUrl,
                }
            }));
        }
    }

    round(num){
        let num1 = Math.round(num)?.toString().slice(0, -3);
        return Number(num1);
    }

    dragSignerStart(signer) {
        this.draggedSigner = signer;
    }

    dragSignerEnd() {
        this.draggedSigner = null;
    }

    dragCCStart(cc) {
        this.draggedCC = cc;
    }

    dragCCEnd() {
        this.draggedCC = null;
    }

    dropSigner() {
        if (this.draggedCC) {
            this.selectedCC = this.selectedCC.filter(x => x.id != this.draggedCC?.id);
            this.selectedSigner = [...this.selectedSigner, this.draggedCC];
        }
    }

    dropCC() {
        if (this.draggedSigner) {
            if(this.draggedSigner?.id == this.appSession.userId){
                this.notify.warn(this.l('YouMustBeSigner'));
                return;
            }
            this.selectedSigner = this.selectedSigner.filter(x => x.id != this.draggedSigner?.id);
            this.selectedCC = [...this.selectedCC, this.draggedSigner];
        }
    }


}
