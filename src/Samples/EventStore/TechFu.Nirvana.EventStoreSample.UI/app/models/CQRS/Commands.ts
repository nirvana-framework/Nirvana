import {Command,Query,PagedResult} from "./Common";
//Common
export class ValidationMessage{constructor(public MessageType: MessageType,public Key: string,public Message: string){}}
export class PaginationQuery{constructor(public PageNumber: number,public ItemsPerPage: number){}}
export enum MessageType{Info=1,Warning=2,Error=3,Exception=4}
//Infrastructure
export class GetVersionQuery extends Query<VersionModel>{constructor(){super('Infrastructure/GetVersion')}}
export class VersionModel{public Version: string;}
export class TestCommand extends Command<TestResult>{constructor(){super('Infrastructure/Test')}}
export class TestResult{public Message: string;}
//ProductCatalog
export class CreateSampleCatalogCommand extends Command<Nop>{constructor(){super('ProductCatalog/CreateSampleCatalog')}}
export enum Nop{NoValue=0}
//Users
