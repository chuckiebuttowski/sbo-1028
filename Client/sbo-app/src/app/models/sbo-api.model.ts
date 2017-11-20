export class APIControllerDocumentation {
    Name: string;
    Description: string;
    IsSecured: boolean;
    BaseURL:string;
    Actions: APIActionDocumentation[];
}

export class APIActionDocumentation {
    Name: string;
    Path: string;
    Method: string;
    Description: string;
    Parameters: APIParameterDocumentation[];
    Response: APIResponseDocumentation;
}

export class APIParameterDocumentation {
    Name: string;
    Type: string;
    IsOptional: boolean;
}

export class APIResponseDocumentation {
    Type: string;
    Description: string;
}