// Бэк: LoginRequestDto.cs / LoginResponseDto.cs.
// TODO: regenerate api-generated.ts (npm run generate:api) after backend restart — схем Auth нет в сгенерированном файле
export interface LoginRequestDto {
    login: string,
    password: string
}

export interface LoginResponseDto {
    token: string
}
