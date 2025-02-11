export interface Account {
    anonUserName: string,
    profileImage: string | null,
    id: string,
    userName: string,
    normalizedUserName: string,
    email: string,
    normalizedEmail: string,
    emailConfirmed: boolean,
    passwordHash: string,
    securityStamp: string,
    concurrencyStamp: string,
    phoneNumber: null,
    phoneNumberConfirmed: boolean,
    twoFactorEnabled: boolean,
    lockoutEnd: null,
    lockoutEnabled: boolean,
    accessFailedCount: 0
}