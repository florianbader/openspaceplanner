export const groupBy = <T, K extends keyof any>(
    list: T[],
    getKey: (item: T) => K,
) =>
    list.reduce(
        (previous, currentItem) => {
            const group = getKey(currentItem);
            if (!previous[group]) {
                previous[group] = [];
            }

            previous[group].push(currentItem);
            return previous;
        },
        {} as Record<K, T[]>,
    );

declare global {
    interface Array<T> {
        groupBy<K extends keyof any>(getKey: (item: T) => K): Record<K, T[]>;
    }
}

Array.prototype.groupBy = function <T, K extends keyof any>(
    getKey: (item: T) => K,
): Record<K, T[]> {
    return groupBy(this, getKey);
};
