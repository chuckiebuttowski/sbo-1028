export class ClientAPI {
    ID: number;
    Name: string = "";
    URL: string = "";
    Description: string = "";
    Params: APIParameter[] = new Array<APIParameter>();
    Action: string;
    SboTransactionType: string;
    ValueType: PostDataValueType;
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
    SingleEntity,
    MultipleEntities
}