export class TransactionLog{
    ID: number;
    Origin: string;
    TransactionNo: string;
    SBOType : SBOType;
    LogDate: Date;
    IsPosted: boolean;
    Remarks: string;
    RawData: TransactionData = new TransactionData();
    Error: ErrorLog = new ErrorLog();
}

export class TransactionData{
    ID: number;
    PostedOn: Date;
    RawData: string;//expected json
}

export class ErrorLog {
    ID: number;
    ErrorCode: number;
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