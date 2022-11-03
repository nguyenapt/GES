import { GssPrincipleAssessment } from "./gss-principle-assessment";
import { GssPrincipleGeneral } from "./gss-principle-general";
import { GssPrincipleUpgradeCriteria } from "./gss-principle-upgrade-criteria";
import { GssPrincipleCompanyContact } from "./gss-principle-company-contact";

export class GssIssueIndicator {
    Id: string;
    Title: string;
    Group: number;
    Assessment: GssPrincipleAssessment;
    General: GssPrincipleGeneral;
    UpgradeCriteria: GssPrincipleUpgradeCriteria;
    CompanyContact: GssPrincipleCompanyContact;
}