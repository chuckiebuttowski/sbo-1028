declare global{
    interface Array<T>{
        toDataTable(pageSize: number): DataTable
    }
}

Array.prototype.toDataTable = function(pageSize: number){
    var dt = new DataTable;
    dt.assemblePager(this, pageSize);
    return dt;
}

class Filter{
    Name:string;
    Value:string;
}

export class DataTable {
    private mainDataSource: any[];

    public DataSource: any[] = new Array<any>();
    public CurrentPage: number = 1;
    public Pages: number[];
    public PageSize: number;
    private totalPages: number;
    private filters: Filter[] = new Array<Filter>();

    assemblePager(dataSource: any[], pageSize: number){
        this.mainDataSource = dataSource;
        this.PageSize = pageSize;
        this.paginate();
    }

    public setCurrentPage(currentPage: number){
        this.CurrentPage = currentPage;
        this.paginate();
    }

    public movePrevious(){
        this.CurrentPage--;
        if(this.CurrentPage < 1){
            this.CurrentPage = 1;
        }
        this.paginate();
    }

    public moveNext(){
        this.CurrentPage++;
        if(this.CurrentPage > this.totalPages){
            this.CurrentPage = this.totalPages;
        }

        this.paginate();
    }

    public filterColumn(colName: string, value:any){

        let filterIdx = this.filters.findIndex(x => x.Name == colName);
        if(filterIdx < 0){
            this.filters.push({Name: colName, Value: value});
        }
        else{
            this.filters[filterIdx].Value = value;
        }

        let filteredDS = this.mainDataSource;

        //update support multi filter
        for(let filter of this.filters){
            filteredDS = filteredDS.filter(x => x[filter.Name].toString().toLowerCase().indexOf(filter.Value.toString()) > -1);
        }

        //filteredDS = this.mainDataSource.filter(x => x[colName].toString().toLowerCase().indexOf(value.toString()) > -1);

        // for(let data of this.mainDataSource){
        //     for(let fld in data){
        //         if(fld == colName){
        //             if(data[fld].toString().toLowerCase().indexOf(value.toString()) > -1){
        //                 filteredDS.push(data);
        //             }
        //         }
        //     }
        // }

        this.CurrentPage = 1;
        this.paginate(filteredDS);
    }

    public clearFilters(){
        this.filters = new Array<Filter>();
        this.CurrentPage =1;
        this.paginate(this.mainDataSource);
    }

    public generalSearch(keyword: string){

    }

    public sortColumn(colName: string){

    }

    private paginate(ds: any[] = null){
        if(ds == null){
            ds = this.mainDataSource;
        }

        let startPage: number, endPage: number;
        this.totalPages = Math.ceil(ds.length / this.PageSize);
        
        this.Pages = new Array<number>();

        if(this.totalPages <= 5){
            startPage = 1;
            endPage = this.totalPages;
        }
        else{
            if(this.CurrentPage <= 3){
                startPage = 1;
                endPage = 5;
            }
            else if(this.CurrentPage + 2 >= this.totalPages){
                startPage = this.totalPages - 4;
                endPage = this.totalPages;
            }
            else{
                startPage = this.CurrentPage - 2;
                endPage = this.CurrentPage + 2;
            }
        }

        for(let i = startPage; i <= endPage; i++){
            this.Pages.push(i);
        }

        let startIndex = (this.CurrentPage - 1) * this.PageSize;
        let endIndex = Math.min(startIndex + this.PageSize - 1, ds.length - 1);
        
        this.DataSource = new Array<any>();
        let ctr = 0;
        for(let data of ds){
            if(ctr >= startIndex && ctr <= endIndex){
                this.DataSource.push(data);
            }
            ctr++;
        }
    }

}