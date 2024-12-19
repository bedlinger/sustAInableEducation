export interface Register {
    email: string
    password: string
    confirmPassword: string
}

export interface RegisterError {
    email: Array<{message: string}>
    password: Array<{message: string}>
    confirmPassword: Array<{message: string}>
}