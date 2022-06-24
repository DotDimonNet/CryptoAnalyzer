## CryptoAnalyzer
Predict best profit from the predictive data for date range

### Fast running:
If you don't have Visual Studio installed, you can open CryptoAnalyzerBuild folder and run [.exe] file.

Main window contains inputs for getting the profit based on basic options (includes balance, treshold, etc.)

For starting work, please, clone and build the solution in Visual Studio. 
Extract csv file with data source to the folder and put valid path into necessary field.
Results will be in text file (results-{datetime}.txt).
### Notice:
Please, be sure that paths that you have for CSV file and folder for result is exists.

### Inputs:
- Balance BTC - contains initial balance of bitcoins for customer
- Balance USD - contains initial balance of dollars for customer
- Treshold BTC to USD - contains percentage division value between real and predictive data
- Treshold USD to BTC - contains percentage division value between real and predictive data
- Limit for action - contains percent of total balance (USD or BTC) that customer can operate on specific time (day)
