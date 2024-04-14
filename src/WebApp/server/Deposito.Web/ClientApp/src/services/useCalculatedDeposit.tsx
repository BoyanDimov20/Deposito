import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

export enum PayoutType {
    Undefined = -1,
    Determined = 0,
    InAdvance = 1,
    Monthly = 2,
    InEnd = 3
}

type CalculatedDepositProps = {
    id: string,
    percent: number,
    period: number,
    currency: string,
    depositedAmount: string,
    amountAfterDeposit: number,
    payoutType: PayoutType
};



export const useCalculatedDeposit = () => {
    const [deposit, setDeposit] = useState<CalculatedDepositProps | null>(null);
    const { id, amount } = useParams();


    useEffect(() => {
        if(id && amount)
            getCalculatedDeposit(id, amount).then(x => setDeposit(x));
    }, []);

    return deposit;
};

async function getCalculatedDeposit(id: string, amount: string) {
    const response = await fetch(`/api/deposit/calculatedeposit?id=${id}&amount=${amount}`);

    if(!response.ok) {
        return null;
    }

    return await response.json() as CalculatedDepositProps;
}