import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

type DepositProps = {
    id: string,
    bankLogo: string,
    description: string,
    percentInterest: number,
    minimalAmount: number,
    currency: string
}
export const useDeposits = (amount : string, currency: string, period: string, paymentType: string) => {

    const [deposits, setDeposits] = useState<DepositProps[]>([]);


    useEffect(() => {
        getDeposits(amount, currency, period, paymentType)
            .then(x => setDeposits(x));

    }, [amount, currency, period, paymentType]);

    return deposits;
};

async function getDeposits(amount: string | undefined, currency: string | undefined, period: string | undefined, paymentType: string | undefined) {
    const response = await fetch(`/api/deposit/getdeposits?amount=${amount}&currency=${currency}&period=${period}&payoutType=${paymentType}`);
    //fetch('/api/weatherforecast');
    if (!response.ok) {
        return [];
    }

    return await response.json() as DepositProps[];
}