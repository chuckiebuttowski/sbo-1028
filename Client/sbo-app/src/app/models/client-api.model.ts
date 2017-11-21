export class ClientAPI {
    ID: number;
    Name: string = "";
    URL: string = "";
    Description: string = "";
    Params: APIParameter[] = new Array<APIParameter>();
    Action: string = null;
    SboTransactionType: string = null;
    ValueType: string = null;
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

export enum PostDataValueType {
    SingleEntity = 0,
    MultipleEntities = 1
}