import { Button, Select, Table, TextInput } from "@mantine/core";
import { useDeposits } from "../services/useDeposits";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";





export default function DepositListings() {

    const { amount, currency, period, paymentType } = useParams();

    const [amountFilter, setAmountFilter] = useState(amount ?? '');
    const [currencyFilter, setCurrencyFilter] = useState(currency ?? null);
    const [periodFilter, setPeriodFilter] = useState(period ?? null);

    const deposits = useDeposits(amountFilter, currencyFilter ?? '', periodFilter ?? '', paymentType ?? '');
    const navigate = useNavigate();

    const calculateHandler = (id: string) => {
        navigate(`/deposit/${id}/${amountFilter}`);
    };

    const rows = deposits.map((deposit) => (
        <tr key={deposit.id}>
            <td><img width="60px" height="30px" src={deposit.bankLogo} /></td>
            <td>{deposit.description}</td>
            <td>{deposit.percentInterest}%</td>
            <td>{deposit.minimalAmount} {deposit.currency}</td>
            <td><Button onClick={() => calculateHandler(deposit.id)}>Изчисли</Button></td>
        </tr>
    ));

    return (
        <div>
            <div className="filters">
                <TextInput
                    placeholder="Въведи размер"
                    label="Размер на депозит"
                    type="number"
                    withAsterisk
                    style={{ marginTop: '10px' }}
                    value={amountFilter}
                    onChange={(e) => setAmountFilter(e.target.value)}
                />
                <Select
                    label="Валута"
                    placeholder="Избери валута"
                    data={[
                        { value: '0', label: 'BGN' },
                        { value: '2', label: 'EUR' },
                        { value: '1', label: 'USD' },
                        { value: '3', label: 'GBP' },
                    ]}
                    style={{ marginTop: '10px' }}
                    value={currencyFilter}
                    onChange={setCurrencyFilter}
                />
                <Select
						style={{ marginTop: '10px' }}
						label="Срок на депозит"
						placeholder="Избери срок за депозит"
						data={[
							{ value: '1', label: '1 месец' },
							{ value: '3', label: '3 месеца' },
							{ value: '6', label: '6 месеца' },
							{ value: '9', label: '9 месеца' },
							{ value: '12', label: '12 месеца' },
							{ value: '18', label: '18 месеца' },
							{ value: '24', label: '24 месеца' },
							{ value: '36', label: '36 месеца' },
							{ value: '48', label: '48 месеца' },
							{ value: '60', label: '60 месеца' },
							{ value: '120', label: '120 месеца' }
						]}
						value={periodFilter}
						onChange={setPeriodFilter}
					/>
            </div>
            <Table>
                <thead>
                    <tr>
                        <th>Банка</th>
                        <th>Описание</th>
                        <th>Процент</th>
                        <th>Минимална сума</th>
                        <th>Действие</th>
                    </tr>
                </thead>
                <tbody>{rows}</tbody>
            </Table>
        </div>
    );
}