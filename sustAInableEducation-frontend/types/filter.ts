export interface OverviewFilter {
    applied: {
        finished: Ref<string>,
        date: Ref<Date | Date[] | (Date | null)[] | null | undefined>,
        sort: {
            subject: Ref<string>,
            direction: Ref<boolean>
        }
    },
    refs: {
        finished: Ref<string>,
        date: Ref<Date | Date[] | (Date | null)[] | null | undefined>,
        sort: {
            subject: Ref<string>,
            direction: Ref<boolean>
        }
    },
    options: {
        finished: Array<string>
        sort: Array<string>
    }
}