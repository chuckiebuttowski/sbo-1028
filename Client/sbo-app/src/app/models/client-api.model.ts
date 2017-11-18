export class ClientAPI {
    ID: number;
    Name: string = "";
    URL: string = "";
    Description: string = "";
    Params: APIParameter[] = new Array<APIParameter>();
    IsActive: boolean;
}

export class APIParameter {
    Name: string = "";
    Type: ParameterType = ParameterType.String;
}

export enum ParameterType {
    String = "String",
    Integer = "Integer",
    Float = "Float",
    Date = "Date",
    Boolean = "Boolean"
}