import { useState } from 'react'
import './App.css'
import { Button, Radio, Select, TextInput } from '@mantine/core'
import { useNavigate } from 'react-router-dom';

function App() {

	const [amount, setAmount] = useState('');
	const [currency, setCurrency] = useState<string | null>(null);
	const [period, setPeriod] = useState<string | null>(null);
	const [paymentType, setPaymentType] = useState<string | null>(null);
	const navigate = useNavigate();


	return (
		<>
			<div style={{ border: '1px solid gray', borderRadius: '20px', padding: '20px' }}>
				<h3 style={{ marginBottom: '20px', textAlign: 'center' }}>Намери депозит</h3>
				<Radio.Group
					left={'auto'}
					label="ВИД ДЕПОЗИТ"
					defaultValue='standard'
					withAsterisk
				>
					<Radio style={{ margin: '10px' }} value="standard" label="СТАНДАРТЕН СРОЧЕН ДЕПОЗИТ" />
					<Radio style={{ margin: '10px' }} value="monthly" label="ДЕПОЗИТ С МЕСЕЧНИ ВНОСКИ" />
				</Radio.Group>
				<div style={{ display: 'flex', flexDirection: 'column' }}>
					<TextInput
						placeholder="Въведи размер"
						label="Размер на депозит"
						type="number"
						withAsterisk
						style={{ marginTop: '10px' }}
						value={amount}
						onChange={(e) => setAmount(e.target.value)}
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
						value={currency}
						onChange={setCurrency}
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
						value={period}
						onChange={setPeriod}
					/>
					<Select
						style={{ marginTop: '10px' }}
						label="Изплащане на лихви"
						placeholder="Избери начин за изплащане"
						data={[
							{ value: '1', label: 'Ежемесечно' },
							{ value: '2', label: 'Годишно' },
							{ value: '3', label: 'На край на период' }
						]}
						value={paymentType}
						onChange={setPaymentType}
					/>

					<Button style={{ marginTop: '20px' }}
					onClick={() => navigate(`/deposit-list/${amount}/${currency}/${period}/${paymentType}`)}
					>
						Търси
					</Button>
				</div>
			</div>
		</>
	)
}

export default App
