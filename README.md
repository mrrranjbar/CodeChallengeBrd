Description of the Task
You are required to provide a production ready solution (Console Application) written in C#.
An XML file containing generator data (see accompanying file 01-Basic.xml) is produced and provided as input into an input folder on a regular basis. 
The solution is required to automatically pick up the received XML file as soon as it is placed in the input folder (location of input folder is set in a configuration file), perform parsing and data processing as appropriate to achieve the following:
1.	It is required to calculate and output the Total Generation Value for each generator.
2.	It is required to calculate and output the generator with the highest Daily Emissions for each day along with the emission value.
3.	It is required to calculate and output Actual Heat Rate for each coal generator. 
The output should be a single XML file in the format as specified by an example accompanying file 01-Basic-Result.xml into an output folder (location of output folder is configured in the Application app.config file). 
Notes:
•	Example filenames input/output: 01-Basic.xml -> 01-Basic-Result.xml
•	There are no XSDs provided for input/output XMLs

Given the time constraints, if any of the ‘production ready’ aspects cannot be implemented please add a note/comments/placeholders for the missing functionality.
Calculation Definitions and Reference Data
Daily Generation Value = Energy x Price x ValueFactor
Daily Emissions = Energy x EmissionRating x EmissionFactor
Actual Heat Rate = TotalHeatInput / ActualNetGeneration
Emissions only apply to fossil fuel generator types e.g. coal and gas. There could be a varying number of generators of a given type in any one GenerationReport.xml file that is produced.
ValueFactor and EmissionsFactor is static data sourced from the accompanying XML file ReferenceData.xml. Note: it is not possible to change static data while the console application is running. 
Generator Types map to factors as follows:
Generator Type #	Value Factor	#  EmissionFactor
Offshore # Wind	# Low	# N/A
Onshore # Wind	# High	# N/A
Gas	# Medium	# Medium
Coal	# Medium	# High
