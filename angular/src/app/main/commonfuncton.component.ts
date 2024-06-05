export class CommonFunction {

    isShowUserProfile(){

        let _u = document.getElementById("kt_app_header");
        if (!_u) return;

        if(window.location.href.indexOf("create-new-request") > -1 ||
           window.location.href.indexOf("add-signature") > -1) {
            _u.style.display = "none";
        }else {
            _u.style.display = 'flex';
        }
    }

    setHeight() {

        let w = window.innerWidth;
        let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group");
        let h_search = 0;
        if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight }

        let h = (window.innerHeight - (55 + 10 + 32 + h_search + 28 + 39)) + 'px'; //top bar header: 12 + 30 + (10)
        document.querySelector<HTMLElement>('simple-ag-grid ag-grid-angular').style.height = h;
    }

    isActive(_active: any){
        if(_active) return _active;
        return 'N';
    }

    isStatus(_active: any, _default: string){
        if(_active) return _active;
        return _default;
    }

    showtime(css_time:string){
        let _d = new Date();
        let _time = document.querySelector<HTMLElement>('.' + css_time);
        if(_time) _time.textContent = _d.getHours() + ":" + _d.getMinutes() + ":" + _d.getSeconds();
    }

    getPercentByQty(_total:number, _Qty:number) {
        return (_total / _Qty) * 100;
    }
    getPercentByQty2(percent:number, total:number) {
        return (((percent / 100) * total) / 100).toFixed(2)
    }


    getQtyByPercent(_total:number, _percent:number) {
        let _Qty = (_total * _percent) / 100;
        //_Qty = Math.floor(_Qty * 100) / 100;
        return _Qty;
    }
    getQtyByPercent2(_qty:number, _percent:number) {
        return (_qty * 100) / _percent;
    }


    numbers:Array<any> = [];
    fornumbers(num:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+1);
        return this.numbers;
    }

    fornumbers_next(num:number, start:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+start);
        return this.numbers;
    }

    numbers_desc:Array<any> = [];
    fornumbers_desc(num:number) {
        this.numbers_desc = Array.from({length:num},(v,k)=>num-k);
        return this.numbers_desc;
    }

    fornumbersRangeDesc(start:number, stop:number, step:number){
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    fornumbersRange(start:number, stop:number, step:number){
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }


}
