export class Helper {
    public static Deserialize(data: string): any {
        return JSON.parse(data, Helper.ReviveDateTime);
    }

    public static ReviveDateTime(value: any): any {
        if (typeof value === 'string') {
            let a = /\/Date\((\d*)\)\//.exec(value);
            if (a) {
                return new Date(+a[1]);
            }
        }

        return value;
    }
}