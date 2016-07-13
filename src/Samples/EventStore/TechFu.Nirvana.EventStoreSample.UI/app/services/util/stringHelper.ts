export module StringHelper{
    export function nullOrEmpty(str:string){
        return (!str || 0 === str.length);
    }

}

