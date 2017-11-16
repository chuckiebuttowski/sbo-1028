import { Pipe, PipeTransform } from '@angular/core';


//models
import { TransactionLog } from '../models/log.model';

@Pipe({
    name: 'transactionLogFilter'
})
export class TransactionLogFilterPipe implements PipeTransform {
    transform(items: TransactionLog[], filter: any ): TransactionLog[] {
        if (!items || !filter) {
            return items;
        }

        let result = items.filter((item: TransactionLog) => this.applyFilter(item, filter));;

        return result;
    }

    applyFilter(value: any, filter: any): boolean {
        for (let field in filter) {
            if(value[field] == filter[field])
            {
                return true;
            }
            // if (filter[field]) {
            //   if (typeof filter[field] === 'string') {
            //     if (value[field].toLowerCase().indexOf(filter[field].toLowerCase()) === -1) {
            //       return false;
            //     }
            //   } else if (typeof filter[field] === 'number') {
            //     if (value[field] !== filter[field]) {
            //       return false;
            //     }
            //   }
            //   else{
            //       if(value[field] !== filter[field]){
            //           return false;
            //       }
            //   }
            // }
          }
          return false;
    }
}