export class Alert {
  id: string;
  message: string;
  alertType: string;
  htmlContent?: string; 

  constructor(init?: Partial<Alert>) {
    Object.assign(this, init);
    this.id = init?.id || '';
    this.message = init?.message || '';
    this.alertType = init?.alertType || '';
    this.htmlContent = init?.htmlContent || ''; 
  }
}
