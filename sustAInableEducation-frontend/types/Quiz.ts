export interface Try {
    questions: QuestionVerified[],
}

export interface QuestionVerified {
    id: string,
    isCorrect: boolean
}

export interface Choice {
    number: number,
    text: string
}

export interface Question {
    id: string,
    choices: Choice[],
    number: number,
    text: string,
    isMultipleResponse: boolean
}

export interface Quiz {
    id: string,
    questions:  Question[],
    title: string,
    numberQuestions: number,
    tries: Try[]
}