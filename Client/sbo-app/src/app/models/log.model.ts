export class TransactionLog{
    ID: string;
    Origin: string;
    TransactionNo: string;
    SBOType : SBOType;
    LogDate: Date;
    IsPosted: boolean;
    Remarks: string;
    Data: TransactionData;
}

export class TransactionData{
    PostedOn: Date;
    RawData: string;//expected json
    Error: ErrorLog;
}

export class ErrorLog {
    Code: number;
    Message: string;
    StackTrace: string;
}

export  enum SBOType {
    JE = 30,
    APV = 18,
    OP = 46,
    IP = 24,
    GI = 60,
    GR = 59,
    GRPO = 20,
    ITM = 4,
    BP = 2,
    GL = 1
}   