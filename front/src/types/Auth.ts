export interface LoginRequestDto {
    login: string,
    password: string
}

export interface LoginResponseDto {
    token: string
}
