export class ApiError extends Error {
  public response: ApiErrorResponse

  constructor(message: string, response: ApiErrorResponse) {
    super(message)
    this.response = response
  }
}

export type ApiErrorResponse = {
    Message: string
    ValidationErrors: FieldError[]
}

export type FieldError = {
    Field: string
    Message: string
}
