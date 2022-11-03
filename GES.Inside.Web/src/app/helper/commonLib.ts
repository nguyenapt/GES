export class CommonLib {
    public static GetId(url: string): number{
        let urlPathSplit = String(url).split("/");
        
        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            if (urlPathSplit[urlPathSplit.length - 1] !== 'Add') {
                return Number(urlPathSplit[urlPathSplit.length - 1]);
            } 
        }
        return 0;
    }
}




