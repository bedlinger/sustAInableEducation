export interface Login {
    email: string
    password: string
}

export interface LoginError {
    email: Array<{message: string}>
    password: Array<{message: string}>
}